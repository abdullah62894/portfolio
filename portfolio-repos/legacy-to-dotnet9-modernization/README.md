# Legacy to .NET 9 Modernization

Flagship modernization demo showing the journey from legacy .NET-style application structure to modern .NET 9 architecture.

## Structure

```text
LegacyApp/
  Controllers/
  Services/
  Repositories/

ModernApp/
  Application/
  Domain/
  Infrastructure/
```

## Before

- Fat controller/service workflow
- Business rules living close to infrastructure
- Synchronous repository style
- Tight coupling between API, service, and persistence code

## After

- Clean Architecture boundaries
- CQRS-style command
- MediatR-compatible mediator boundary
- Dependency injection
- Domain entity owns business rules
- Async repository interface

## Why this belongs on the portfolio

This demo maps directly to enterprise migration work: preserve business behavior, carve out domain rules, introduce dependency injection, and move toward testable .NET 9 APIs without exposing proprietary code.
