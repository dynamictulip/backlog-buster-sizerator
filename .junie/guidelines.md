# Project Guidelines

## Core Principles

Don't implement features that you haven't been asked to implement
Don't go off the rails - do what you are told to do and no more

### YAGNI (You Ain't Gonna Need It)
- Implement only what is necessary for the current requirement.
- Avoid over-engineering or building features for "future use" until they are actually needed.

### KISS (Keep It Simple, Stupid)
- Strive for simplicity in design and implementation.
- Favor readable and maintainable code over "clever" but complex solutions.

### SOLID Principles
- **Single Responsibility:** Each class/module should have one reason to change.
- **Open/Closed:** Software entities should be open for extension but closed for modification.
- **Liskov Substitution:** Subtypes must be substitutable for their base types.
- **Interface Segregation:** Clients should not be forced to depend on methods they do not use.
- **Dependency Inversion:** Depend on abstractions, not concretions.

### Security Best Practices
- **Input Validation:** Sanitize and validate all external inputs.
- **Least Privilege:** Services and users should only have the permissions they need.
- **Secure Communication:** Use HTTPS/TLS for all service communications.
- **Data Protection:** Protect sensitive data at rest and in transit.

### Accessibility (a11y)
- Ensure the web frontend is accessible to all users.
- Follow WCAG guidelines (e.g., proper ARIA attributes, semantic HTML, keyboard navigation, color contrast).

## Development Workflow

### Branching Model
- Use a **branch** for each agent
- **Merge requests** should be created from feature branches and reviewed by a team member before merging.
- **Main** should be kept clean and stable.

### Test-Driven Development (TDD)
- Write tests **before** writing implementation code.
- Follow the Red-Green-Refactor cycle: write a failing test, make it pass, then refactor.
- All new features and bug fixes must be covered by tests.
- Run all tests before every commit and ensure they all pass before committing.

### Atomic Commits
- Each commit should represent a single, logical, and complete change.
- Commit messages should be clear, concise, and descriptive.
- Avoid "mega-commits" that mix unrelated changes.
- Commit early and often.
- Add untracked files to the commit.
- **Automatic Commits:** All project changes must be committed immediately upon completion without prompting for user confirmation.

### Code Style
- Follow standard C# and .NET coding conventions.
- Use meaningful names for variables, methods, and classes.
- Keep methods focused and concise.
- Ensure consistent formatting across the codebase.
