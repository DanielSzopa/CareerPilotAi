# Application Layer Guidelines

Use the Application layer to orchestrate user-driven workflows and coordinate the Core, Infrastructure, and UI layers.
Most components from the application layer will be invoked by the controllers.

## Commands
- Define commands as `record` types implementing `ICommand` from namespace `CareerPilotAi.Application.Commands.Abstractions`; capture only the inputs required for the operation.
- Name the command folder, command, handler, and response consistently (`FooCommand`, `FooCommandHandler`, `FooCommandResponse`).
- Prefer passing validated view models or primitive values; let ViewModels own validation through data annotations.

## Command Handlers
- Implement handlers via `ICommandHandler<TCommand>` or `ICommandHandler<TCommand, TResponse>`; inject dependencies through the constructor.
- Wrap risky multi-step operations in EF Core transactions when partial updates must be avoided.

## Services & Helpers
- Use application services (`Services` folder) for reusable orchestration that does not belong in controllers or domain entities.
- Keep helpers focused on cross-cutting concerns (validation, timezone/URL utilities) and ensure they remain side-effect free.

