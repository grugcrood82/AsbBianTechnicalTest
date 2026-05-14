# Graph-Backed Entitlement Service

.NET 8 Azure Functions + Neo4j. Checks if a user has permission to access a resource by traversing their Identity → Role → Entitlement → Resource path.

## The Problem

Design and implement the core of a graph-backed entitlement check service.

A financial organisation needs a centralised entitlement system that answers one question for any of its applications.

Identities are customers represented by a unique id. Access is granted through Party Roles and Entitlements. They need to conform to the *BIAN standards*.

**Task - Design and implement a working service that:**
- Uses a graph database (Neo4j preferred) to model the identity -> role -> permissions -> resource traversal.
- Exposes a REST endpoint to evaluate an entitlement check, accepts a subject, a permission name, and a resource identifier; returns allow/deny, a reason, and which permission was granted.
- Includes seed scripts or an endpoint that loads enough demo data to explain how it works.
- Writes unit tests for the check path covering the granted check, denied check etc.

**Constraints**
```
Don't use a pre-built authorisation library. The point is to see how you model the problem, not which library you can wire in.
```
**Non-functional Requirements** 

- Write runnable code using .NET 8.0 or .NET 10 and Neo4j as the Graph DB. Include a README file to explain some of the setup.


## Analysis

```
Identity (CUST-001)
    [:HAS_ROLE]
        ↓
    PartyRole (AccountHolder)
        [:ASSIGNS]
            ↓
        Entitlement (VIEW_BALANCE)
            [:GRANTS_ACCESS_TO]
                ↓
            Resource (ACC-12345)
```

One Cypher query to rule them all:
```cypher
MATCH path = (i:Identity {id: $subjectId})
          -[:HAS_ROLE]->(pr:PartyRole)
          -[:ASSIGNS]->(e:Entitlement)
          -[:GRANTS_ACCESS_TO]->(r:Resource {resourceId: $resourceId})
WHERE e.permission = $permission
RETURN i.id, pr.roleType, e.name, e.permission, r.resourceId
LIMIT 1
```

## Setup

```bash
# Start Neo4j
docker-compose up -d neo4j

# Seed test data
POST http://localhost:7071/api/entitlement/seed

# Try it
POST http://localhost:7071/api/entitlement/check
{
  "subjectId": "CUST-001",
  "permission": "VIEW_BALANCE",
  "resourceId": "ACC-12345"
}
```

## Project Layout

```
EntitlementsService/
├── Models/              # Domain objects (Identity, Role, Entitlement, Resource)
├── Services/             # Business logic + Neo4j driver wrapper
├── Functions/            # HTTP entry points
├── Configuration/        # Neo4j connection settings
├── Constants/            # Relationship types, messages
└── Data/                 # Seed data for local testing

AsbTechnicalBianComplientTest/
├── Tests/                # EntitlementService + SeedData tests
└── Fixtures/             # Testcontainers Neo4j setup
```
`N.B. The security for the API can be delegated to APIM in Azure or similar technology in other cloud platforms. The goal for microservices is to focus on a small area of expertise. The Security concern is handled by another service.`

## Design Decisions

**Azure Functions instead of ASP.NET Core?**  
Single responsibility service—just entitlement checks. Azure Functions keeps it deployable as one unit without the overhead of a full web framework.

**Singleton Neo4jService?**  
The driver is thread-safe and expensive to initialize. One instance per app lifecycle, shared across invocations. EntitlementService is scoped (per-request) because... actually, it could probably be singleton too, but scoped felt safer in case we add request-specific state later.

**Testcontainers?**  
Integration tests need a real Neo4j. Spinning up a container per test run is slow but CI-compatible and hermetic. The assembly-level fixture keeps container lifecycle simple.

## API

### POST /api/entitlement/check

```json
// Request
{
  "subjectId": "CUST-001",
  "permission": "VIEW_BALANCE",
  "resourceId": "ACC-12345"
}

// Allowed response
{
  "allowed": true,
  "reason": "Permission VIEW_BALANCE on resource ACC-12345 granted via role AccountHolder and entitlement ViewAccountBalance",
  "grantedEntitlement": "ViewAccountBalance",
  "path": ["CUST-001", "AccountHolder", "ViewAccountBalance", "ACC-12345"]
}

// Denied response
{
  "allowed": false,
  "reason": "No entitlement found granting VIEW_BALANCE on ACC-12345 for subject CUST-001",
  "grantedEntitlement": null,
  "path": []
}
```

### POST /api/entitlement/seed

Loads demo data. Returns `{ "success": true, "nodesCreated": 13 }`.

## Configuration

| Variable | Default | Notes |
|----------|---------|-------|
| NEO4J_URI | neo4j://localhost:7687 | Bolt address |
| NEO4J_USERNAME | neo4j | |
| NEO4J_PASSWORD | password | Change in production |
| NEO4J_DATABASE | neo4j | |

## Testing

```bash
dotnet test
```

## Demo Data

| Subject | Role | Permission | Resource | Access? |
|---------|------|------------|----------|---------|
| CUST-001 | AccountHolder | VIEW_BALANCE | ACC-12345 | Yes |
| CUST-001 | AccountHolder | TRANSFER_FUNDS | ACC-12345 | Yes |
| CUST-001 | BusinessOwner | VIEW_DASHBOARD | DASHB-001 | Yes |
| CUST-002 | AccountHolder | VIEW_BALANCE | ACC-12345 | Yes |
| CUST-002 | ThirdParty | VIEW_HISTORY | ACC-12345 | Yes |

## Things That Could Be Improved

- Add Redis caching for frequently-checked entitlements
- Bulk entitlement check endpoint (check multiple per request)
- Wildcard permissions like `ADMIN_*` matching `ADMIN_READ`, `ADMIN_WRITE`
- Time-based entitlements that expire
- Delegation: user A acting on behalf of user B for specific resources
- More granular error responses (not just generic 500s)

## Dependencies

| Package | Purpose |
|---------|---------|
| Neo4j.Driver 6.0.0 | Graph DB connectivity |
| Azure Functions.Worker | Function runtime |
| Testcontainers 3.6.0 | Containerized testing |
| Testcontainers.Neo4j 3.1.0 | Neo4j test container |
| MSTest.Sdk 4.0.2 | Unit testing framework |

## Troubleshooting

**Neo4j won't connect:**
```bash
docker ps | findstr neo4j
docker-compose restart neo4j
```

**Tests fail:**
Make sure Docker is running. Tests need it for Testcontainers.

**Build errors:**
```bash
dotnet clean && dotnet build
```

### BIAN Standards
This has been a new term that I had to quickly familiarize myself with. Upon digging deep into it, it quickly became apparent that the emphasis of the standards is interoperability and reusability. Also, the mainstream best practices like DDD (Eric Evans) are at the heart of the development standards. It also speaks of microservices-oriented architecture which in effect breaks down business domains into reusable units. E.g. an onboarding experience service can be used for distinct products like everyday banking and insurance. This speeds up development by allowing product developers to focus on the core of the product and leave boilerplate setup to expert services.

### Considerations
This exercise did not leave much room for DDD and thus the models are anemic. To fully comply with the reusability principle, I chose to use Azure Functions and strip out the heavy overhead of a full framework. I also made this deployable in Docker to make the solution more portable.

### Other Considerations
- CI/CD: Azure DevOps
- Release Management: Gated release with approval process
- IaC: Terraform (HashiCorp)