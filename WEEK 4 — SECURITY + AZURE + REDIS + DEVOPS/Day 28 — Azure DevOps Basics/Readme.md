# Day 28 — Azure DevOps Basics + Git

Today we'll cover **Azure DevOps fundamentals** and the Git workflow you need as a .NET developer.

The overall picture is:

```text
Developer
   ↓
Git
   ↓
Local Repository
   ↓
Remote Repository
   ↓
Azure DevOps
   ├── Repos
   ├── Boards
   ├── Pipelines
   ├── Artifacts
   └── Test Plans
```

---

# 1. What is Azure DevOps?

**Azure DevOps** is Microsoft's set of services for planning, developing, testing, and delivering software.

It provides:

```text
Azure DevOps
│
├── Boards
├── Repos
├── Pipelines
├── Test Plans
└── Artifacts
```

You can use it for the complete software development lifecycle:

```text
Requirement
    ↓
Planning
    ↓
Development
    ↓
Code Review
    ↓
Build
    ↓
Test
    ↓
Deployment
```

---

# 2. Azure DevOps vs Git

Don't confuse them.

### Git

Git is a **distributed version control system**.

It manages:

```text
Code
Commits
Branches
Merges
History
```

### Azure DevOps

Azure DevOps is a **development platform** containing services including:

```text
Repositories
Work items
CI/CD
Testing
Packages
```

So:

```text
Git
 ↓
Version control

Azure DevOps
 ↓
Complete development platform
```

Azure DevOps Repos can host Git repositories.

---

# 3. Azure DevOps Organization

An **Organization** is the top-level Azure DevOps container.

Example:

```text
Company
   ↓
Azure DevOps Organization
```

Conceptually:

```text
https://dev.azure.com/<organization>
```

An organization can contain multiple projects.

```text
Organization
│
├── Project A
├── Project B
└── Project C
```

---

# 4. Azure DevOps Project

A **Project** is a workspace for a particular product/application/team.

Example:

```text
Organization
     ↓
EmployeeManagement
```

Inside the project you'll find services such as:

```text
EmployeeManagement
│
├── Boards
├── Repos
├── Pipelines
├── Test Plans
└── Artifacts
```

---

# 5. Organization vs Project

Remember:

```text
Organization
    ↓
Contains Projects

Project
    ↓
Contains development resources
```

Example:

```text
MyCompany
   │
   ├── EmployeeManagement
   │
   ├── PayrollSystem
   │
   └── InventorySystem
```

---

# 6. Azure Repos

**Azure Repos** provides source control repositories.

For your .NET application:

```text
Day28AzureDevOps
      ↓
Git Repository
      ↓
Azure Repos
```

You can use Git commands:

```bash
git clone
git add
git commit
git push
git pull
```

---

# 7. Azure Boards

**Azure Boards** is used for planning and tracking work.

Typical work items include:

```text
Epic
Feature
User Story
Task
Bug
```

Example:

```text
Epic
 └── Employee Management
      ├── Employee CRUD
      ├── Employee Search
      └── Employee Authentication
```

---

# 8. Typical Board Workflow

Example:

```text
New
 ↓
Active
 ↓
Resolved
 ↓
Closed
```

The exact workflow can vary depending on your Azure DevOps process/template.

---

# 9. User Story

A user story describes functionality from the user's perspective.

Example:

```text
As an HR administrator,
I want to add an employee,
so that I can maintain employee records.
```

Then create tasks:

```text
User Story
   ↓
Create Employee API
   ↓
Create SQL procedure
   ↓
Create repository
   ↓
Create service
   ↓
Create controller
   ↓
Write tests
```

---

# 10. Azure Pipelines

**Azure Pipelines** provides CI/CD automation.

CI = **Continuous Integration**

CD = **Continuous Delivery / Continuous Deployment**

Typical pipeline:

```text
Developer
   ↓
git push
   ↓
Azure Repos
   ↓
Pipeline Trigger
   ↓
Restore
   ↓
Build
   ↓
Test
   ↓
Publish
   ↓
Deploy
```

For a .NET application:

```text
Restore
 ↓
Build
 ↓
Test
 ↓
Publish
 ↓
Deploy
```

---

# 11. CI — Continuous Integration

Suppose five developers work on the same application.

Every developer pushes code.

```text
Developer A ─┐
Developer B ─┤
Developer C ─┼──→ Repository
Developer D ─┤
Developer E ─┘
                  ↓
               Pipeline
                  ↓
                Build
                  ↓
                 Test
```

The purpose is to detect integration/build/test problems early.

---

# 12. CD — Continuous Delivery / Deployment

After successful build and tests:

```text
Build
 ↓
Test
 ↓
Package
 ↓
Deploy
```

Example:

```text
Development
     ↓
Test
     ↓
Staging
     ↓
Production
```

Whether deployment is automatic or requires approval depends on the pipeline design.

---

# 13. Azure Artifacts

**Azure Artifacts** is used to manage packages.

Examples:

```text
NuGet
npm
Maven
Python packages
Universal Packages
```

For your .NET work, the most relevant one is:

```text
NuGet
```

Example:

```text
InternalCompanyLibrary
        ↓
Azure Artifacts
        ↓
.NET applications
```

This is useful when a company has reusable internal libraries.

---

# 14. Azure Test Plans

**Azure Test Plans** helps teams plan and manage manual testing.

It can be used for:

* Test cases
* Test suites
* Test plans
* Manual testing
* Tracking test results

Conceptually:

```text
Requirement
    ↓
Test Case
    ↓
Execute Test
    ↓
Pass / Fail
```

---

# 15. Azure DevOps Complete Picture

```text
                 Azure DevOps
                      │
       ┌──────────────┼──────────────┐
       ↓              ↓              ↓
     Boards          Repos        Pipelines
       │              │              │
       ↓              ↓              ↓
   Planning         Code          CI/CD
                                     
       ┌──────────────┴──────────────┐
       ↓                             ↓
   Test Plans                     Artifacts
       │                             │
       ↓                             ↓
    Testing                        Packages
```

---

# 16. Git

Now let's understand Git.

**Git** is a distributed version control system.

It tracks changes to files over time.

For example:

```text
Day 1
 ↓
Create project

Day 2
 ↓
Add Employee API

Day 3
 ↓
Add Authentication

Day 4
 ↓
Add Redis
```

Git lets you track these changes through commits.

---

# 17. Why Git?

Without Git:

```text
Project
Project_Final
Project_Final_New
Project_Final_New_2
Project_Final_Really_Final
```

With Git:

```text
Commit 1
 ↓
Commit 2
 ↓
Commit 3
 ↓
Commit 4
```

You have history.

---

# 18. Git Architecture

This is extremely important:

```text
Working Directory
       ↓
    Staging
       ↓
Local Repository
       ↓
Remote Repository
```

For Azure DevOps:

```text
Working Directory
       ↓
Staging Area
       ↓
Local Git Repository
       ↓
Azure Repos
```

---

# 19. Working Directory

This is the actual project folder you're working on.

Example:

```text
D:\dotnet-hands-on\EmployeeManagement
```

You edit:

```text
Program.cs
Employee.cs
EmployeeService.cs
```

These changes initially exist in your working directory.

---

# 20. Staging Area

The staging area contains changes you've selected for the next commit.

Command:

```bash
git add .
```

Flow:

```text
Working Directory
       ↓
git add
       ↓
Staging
```

You can also add a specific file:

```bash
git add Program.cs
```

---

# 21. Local Repository

When you commit:

```bash
git commit -m "Add employee API"
```

the staged changes become part of your local Git history.

```text
Staging
   ↓
git commit
   ↓
Local Repository
```

---

# 22. Remote Repository

The remote repository is hosted somewhere such as:

```text
Azure Repos
GitHub
GitLab
```

For your Azure DevOps learning:

```text
Local Repository
       ↓
git push
       ↓
Azure Repos
```

---

# 23. `git init`

Creates a new Git repository in your current folder.

```bash
git init
```

Example:

```bash
cd D:\dotnet-hands-on\Day28AzureDevOps
git init
```

Git creates a hidden:

```text
.git
```

directory.

Conceptually:

```text
Day28AzureDevOps
│
├── Program.cs
├── Day28AzureDevOps.csproj
└── .git
```

---

# 24. `git status`

Shows the current state of your working directory and staging area.

```bash
git status
```

Example:

```text
Untracked files:
    Program.cs
    Employee.cs
```

After:

```bash
git add .
```

they become staged.

---

# 25. `git add`

Moves changes to staging.

Add one file:

```bash
git add Program.cs
```

Add multiple:

```bash
git add Program.cs Employee.cs
```

Add everything:

```bash
git add .
```

Flow:

```text
Working Directory
       ↓
git add
       ↓
Staging Area
```

---

# 26. `git commit`

Creates a commit in the local repository.

```bash
git commit -m "Add employee model"
```

A good commit message explains the change.

Good:

```text
Add employee CRUD API
```

Bad:

```text
changes
```

---

# 27. Commit Flow

```text
Edit files
   ↓
git status
   ↓
git add .
   ↓
git status
   ↓
git commit -m "Add employee API"
```

---

# 28. `git clone`

Downloads an existing remote repository and creates a local copy.

```bash
git clone <repository-url>
```

For example, conceptually:

```bash
git clone https://dev.azure.com/company/EmployeeManagement/_git/EmployeeManagement
```

After cloning:

```text
Azure Repos
     ↓
git clone
     ↓
Local Repository
```

---

# 29. `git push`

Uploads local commits to the remote repository.

```bash
git push
```

Flow:

```text
Local Repository
       ↓
git push
       ↓
Remote Repository
```

Example:

```bash
git add .
git commit -m "Add employee API"
git push
```

---

# 30. `git pull`

Gets changes from the remote repository and integrates them into your current branch.

```bash
git pull
```

Conceptually:

```text
Remote Repository
       ↓
git pull
       ↓
Local Repository + Working Tree
```

Under the hood, `git pull` is essentially a fetch followed by integration, commonly merge or rebase depending on configuration/flags.

---

# 31. `git fetch`

Even though it wasn't in your required list, you should know it.

```bash
git fetch
```

Downloads remote updates but doesn't integrate them into your current branch.

```text
Remote
  ↓
git fetch
  ↓
Local remote-tracking references
```

Then you can inspect and decide how to integrate them.

Compare:

```text
git fetch
→ Download remote information

git pull
→ Fetch + integrate
```

---

# 32. Git Branch

A branch is an independent line of development.

Example:

```text
main
  │
  ├── feature/employee-api
  │
  └── feature/authentication
```

Branches allow developers to work on different features without immediately changing the main branch.

---

# 33. `git branch`

List branches:

```bash
git branch
```

Example:

```text
* main
  feature/employee-api
```

The `*` indicates the currently checked-out branch.

Create a branch:

```bash
git branch feature/employee-api
```

---

# 34. `git checkout`

Historically used to switch branches.

```bash
git checkout feature/employee-api
```

You can also create and switch:

```bash
git checkout -b feature/employee-api
```

Modern Git generally recommends `git switch` for branch switching because it makes the intent clearer.

---

# 35. `git switch`

Switch branch:

```bash
git switch main
```

Create and switch:

```bash
git switch -c feature/employee-api
```

This is the modern command you should prefer for branch operations.

---

# 36. Typical Branch Workflow

```bash
git switch main
git pull

git switch -c feature/employee-api
```

Work:

```bash
git add .
git commit -m "Add employee API"
```

Then:

```bash
git push -u origin feature/employee-api
```

Now the remote has:

```text
feature/employee-api
```

---

# 37. Why Use Feature Branches?

Instead of:

```text
Everyone
   ↓
main
```

use:

```text
                  main
                   ↑
                   │
        Pull Request / Merge
                   │
                   │
feature/employee-api
```

This provides:

* Code review
* Safer integration
* Isolated development
* Easier collaboration

---

# 38. `git merge`

Merge one branch into another.

Suppose:

```text
main
feature/employee-api
```

First switch to main:

```bash
git switch main
```

Then:

```bash
git merge feature/employee-api
```

Flow:

```text
feature/employee-api
        ↓
      merge
        ↓
       main
```

---

# 39. Merge Example

Before:

```text
A---B---C  main
     \
      D---E  feature
```

After merge, Git may create a merge commit:

```text
A---B---C------M  main
     \         /
      D---E---
```

The exact graph depends on the branch history and merge strategy.

---

# 40. `git rebase`

Rebase moves/replays your branch commits on top of another base.

Example:

Before:

```text
A---B---C  main
     \
      D---E  feature
```

Run:

```bash
git switch feature
git rebase main
```

Conceptually:

```text
A---B---C  main
         \
          D'---E'  feature
```

The feature commits are replayed on top of the latest `main`.

---

# 41. Merge vs Rebase

### Merge

```text
git merge main
```

Preserves the branch history and may create a merge commit.

### Rebase

```text
git rebase main
```

Replays commits onto a new base and creates new commit IDs.

Memory:

```text
Merge
→ Combine histories

Rebase
→ Replay my commits on a new base
```

---

# 42. Important Rebase Warning

Do not casually rebase commits that other people are already depending on in a shared branch.

Why?

Rebase changes commit history.

If you have:

```text
D
E
```

after rebase they become:

```text
D'
E'
```

Different commit identities.

A safe rule for beginners:

> Rebase your own local/private feature work freely when appropriate; be cautious about rebasing shared/public history.

---

# 43. Git Workflow for Your .NET Project

Suppose you create:

```text
Day28AzureDevOps
```

### Step 1 — Initialize

```bash
git init
```

### Step 2 — Check

```bash
git status
```

### Step 3 — Add

```bash
git add .
```

### Step 4 — Commit

```bash
git commit -m "Initial .NET project"
```

### Step 5 — Add Azure DevOps remote

```bash
git remote add origin <azure-repos-url>
```

### Step 6 — Push

```bash
git push -u origin main
```

---

# 44. `git remote`

Check remote:

```bash
git remote -v
```

Example:

```text
origin  <repository-url> (fetch)
origin  <repository-url> (push)
```

`origin` is simply the conventional name for the default remote.

---

# 45. Complete First-Time Azure Repos Flow

After creating an empty Git repository in Azure Repos:

```bash
cd D:\dotnet-hands-on\Day28AzureDevOps

git init

git add .

git commit -m "Initial commit"

git branch -M main

git remote add origin <AZURE_REPOS_URL>

git push -u origin main
```

Then:

```text
Local Repository
       ↓
git push
       ↓
Azure Repos
```

---

# 46. Existing Azure Repo → Local Machine

If the repository already exists:

```bash
git clone <AZURE_REPOS_URL>
```

Then:

```bash
cd EmployeeManagement
```

Create branch:

```bash
git switch -c feature/employee-search
```

Work:

```bash
git add .
git commit -m "Add employee search"
```

Push:

```bash
git push -u origin feature/employee-search
```

---

# 47. Typical Team Workflow

A realistic workflow:

```text
main
  │
  ├── feature/employee-crud
  ├── feature/authentication
  └── bugfix/employee-validation
```

Developer:

```text
1. git pull
2. Create branch
3. Make changes
4. git status
5. git add
6. git commit
7. git push
8. Create Pull Request
9. Code Review
10. Merge
```

---

# 48. Pull Request

A **Pull Request (PR)** is a request to merge changes from one branch into another.

Example:

```text
feature/employee-api
          ↓
     Pull Request
          ↓
         main
```

A PR commonly includes:

* Code review
* Automated build
* Automated tests
* Comments
* Approval
* Merge

Azure DevOps Repos supports pull requests.

---

# 49. Git + Azure Boards

You can connect development work with work items.

For example:

```text
User Story #125
      ↓
Task #126
      ↓
feature/employee-search
      ↓
Commit
      ↓
Pull Request
      ↓
Pipeline
```

This gives traceability from requirement to code and delivery.

---

# 50. Git + Azure Pipelines

Once code is pushed:

```text
Developer
    ↓
git push
    ↓
Azure Repos
    ↓
Azure Pipeline
    ↓
dotnet restore
    ↓
dotnet build
    ↓
dotnet test
    ↓
dotnet publish
    ↓
Deployment
```

---

# 51. .NET Pipeline Commands

A basic .NET CI pipeline typically performs operations conceptually like:

```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
dotnet publish --configuration Release
```

The exact pipeline YAML depends on your project and deployment target.

---

# 52. `.gitignore`

For .NET projects, you should not commit build output and many IDE-generated files.

Create a `.gitignore`.

Typical entries include:

```text
bin/
obj/
.vs/
*.user
*.suo
```

You can use the standard Visual Studio/.NET `.gitignore` template rather than maintaining a tiny custom list manually.

Important:

```text
❌ bin
❌ obj
❌ .vs
❌ local secrets
```

should generally not be committed.

---

# 53. Git Status Example

Suppose you modify:

```text
Program.cs
EmployeeService.cs
```

Run:

```bash
git status
```

You might see:

```text
modified:
    Program.cs
    EmployeeService.cs
```

Then:

```bash
git add Program.cs
```

Now:

```text
Changes to be committed:
    Program.cs

Changes not staged:
    EmployeeService.cs
```

This demonstrates why staging exists.

---

# 54. Selective Commit

You don't have to commit every changed file.

Example:

```bash
git add EmployeeService.cs
git commit -m "Update employee service"
```

`Program.cs` remains unstaged.

This is useful for creating focused commits.

---

# 55. Git Log

Another important command not in your required list:

```bash
git log
```

Shows commit history.

A shorter version:

```bash
git log --oneline
```

Example:

```text
a82f1c2 Add employee search
b72d901 Add employee service
a10f553 Initial commit
```

---

# 56. Git Diff

Another essential command:

```bash
git diff
```

Shows unstaged changes.

For staged changes:

```bash
git diff --staged
```

Useful workflow:

```bash
git status
git diff
git add .
git diff --staged
git commit -m "..."
```

---

# 57. Undoing Changes — Basic Awareness

You should know these, although they're beyond your required command list.

Discard an unstaged working-tree change to a file:

```bash
git restore Program.cs
```

Unstage a staged file:

```bash
git restore --staged Program.cs
```

Be careful with destructive commands, especially when changes haven't been committed.

---

# 58. `git pull` and Conflicts

Suppose:

```text
Developer A
    ↓
push
    ↓
Azure Repos
```

Meanwhile Developer B has old code.

Developer B:

```bash
git push
```

may be rejected because the remote has commits B doesn't have.

Usually:

```bash
git pull
```

then resolve any conflicts, commit the resolution if needed, and push again.

---

# 59. Merge Conflict

Suppose two developers change the same lines.

Git may show:

```text
<<<<<<< HEAD
Console.WriteLine("Hello");
=======
Console.WriteLine("Welcome");
>>>>>>> feature
```

You manually decide the correct final content, then:

```bash
git add .
git commit -m "Resolve merge conflict"
```

---

# 60. Rebase Conflict

During:

```bash
git rebase main
```

you may encounter conflicts.

After fixing a conflict:

```bash
git add .
git rebase --continue
```

To cancel the rebase:

```bash
git rebase --abort
```

---

# 61. Git Commands — Your Required List

## `git init`

Create repository.

```bash
git init
```

## `git clone`

Copy remote repository.

```bash
git clone <url>
```

## `git status`

Check state.

```bash
git status
```

## `git add`

Stage changes.

```bash
git add .
```

## `git commit`

Create local commit.

```bash
git commit -m "Add employee API"
```

## `git push`

Upload commits.

```bash
git push
```

## `git pull`

Fetch and integrate remote changes.

```bash
git pull
```

## `git branch`

Manage/list branches.

```bash
git branch
```

## `git checkout`

Older/general command; can switch branches.

```bash
git checkout main
```

## `git switch`

Modern branch-switching command.

```bash
git switch main
```

## `git merge`

Combine histories.

```bash
git merge feature/employee
```

## `git rebase`

Replay commits onto a new base.

```bash
git rebase main
```

---

# 62. Most Important Git Flow

Memorize this:

```text id="f8q4iw"
             Working Directory
                    ↓
                 git add
                    ↓
               Staging Area
                    ↓
                git commit
                    ↓
              Local Repository
                    ↓
                 git push
                    ↓
             Remote Repository
```

To receive other people's changes:

```text id="h2x2i9"
Remote Repository
       ↓
    git pull
       ↓
Local Repository / Working Tree
```

---

# 63. Git Branch Flow

```text id="0ndpzs"
main
 │
 ├── feature/login
 │
 ├── feature/employee-api
 │
 └── bugfix/validation
```

Typical:

```bash
git switch main
git pull

git switch -c feature/employee-api

# Make changes

git status
git add .
git commit -m "Add employee API"

git push -u origin feature/employee-api
```

Then:

```text
feature/employee-api
        ↓
Pull Request
        ↓
Code Review
        ↓
Pipeline
        ↓
main
```

---

# 64. Azure DevOps Complete Development Flow

Now combine everything:

```text
                  Azure DevOps
                       │
          ┌────────────┼────────────┐
          │            │            │
        Boards        Repos      Pipelines
          │            │            │
          ↓            ↓            ↓
     User Story       Git          CI/CD
          │            │            │
          └──────┬─────┘            │
                 ↓                  │
              Developer             │
                 ↓                  │
              Branch                │
                 ↓                  │
               Commit               │
                 ↓                  │
                Push ───────────────┘
                                    ↓
                                  Build
                                    ↓
                                  Test
                                    ↓
                                 Publish
                                    ↓
                                 Deploy
```

And:

```text
Azure Artifacts
      ↓
NuGet / Packages

Azure Test Plans
      ↓
Manual testing / test management
```

---

# 65. Day 28 Practical Exercise

For your existing `.NET hands-on` roadmap, create:

```text
Day28AzureDevOps
```

### Step 1 — Create project in Visual Studio

```text
ASP.NET Core Web API
.NET 8
```

### Step 2 — Initialize Git

```bash
git init
```

### Step 3 — Add `.gitignore`

Use a standard Visual Studio/.NET `.gitignore`.

### Step 4 — Initial commit

```bash
git add .
git commit -m "Initial .NET 8 API project"
```

### Step 5 — Create Azure DevOps project

```text
Azure DevOps
    ↓
Organization
    ↓
New Project
    ↓
Day28AzureDevOps
```

### Step 6 — Create Azure Repos repository

```text
Repos
 ↓
Files
 ↓
Repository
```

### Step 7 — Connect local repository

```bash
git remote add origin <AZURE_REPOS_URL>
```

### Step 8 — Push

```bash
git branch -M main
git push -u origin main
```

---

# 66. Practice Feature Branch

Create:

```bash
git switch -c feature/employee-api
```

Make a change.

Then:

```bash
git status
```

Stage:

```bash
git add .
```

Commit:

```bash
git commit -m "Add employee API"
```

Push:

```bash
git push -u origin feature/employee-api
```

Then create a Pull Request:

```text
feature/employee-api
        ↓
Pull Request
        ↓
main
```

---

# 67. Day 28 Interview Questions

### What is Git?

A distributed version control system.

### What is Azure DevOps?

A Microsoft platform providing services for planning, source control, CI/CD, testing, and package management.

### What is Azure Repos?

The Azure DevOps service for hosting source-control repositories.

### What is Azure Boards?

Used for planning and tracking work items.

### What are Azure Pipelines?

Used to automate build, test, and deployment workflows.

### What are Azure Artifacts?

Used to host and manage software packages.

### What are Azure Test Plans?

Used to manage test plans, test cases, suites, and manual testing activities.

### What is the staging area?

The area containing changes selected for the next commit.

### What does `git commit` do?

Creates a commit in the local repository from staged changes.

### What does `git push` do?

Uploads local commits to a remote repository.

### What does `git pull` do?

Fetches remote changes and integrates them into the current local branch.

### Difference between `git fetch` and `git pull`?

```text
fetch
→ Download remote updates

pull
→ Fetch + integrate
```

### `git checkout` vs `git switch`?

`git checkout` is an older/general command with multiple purposes. `git switch` is the modern command specifically intended for switching/creating branches.

### Merge vs Rebase?

```text
Merge
→ Combines histories

Rebase
→ Replays commits on a new base
```

### What is a branch?

An independent line of development.

### What is a Pull Request?

A request to review and merge changes from one branch into another.

---

# 68. Final Day 28 Cheat Sheet

## Azure DevOps

```text
Organization
    ↓
Project
    ↓
├── Boards       → Work tracking
├── Repos        → Source control
├── Pipelines    → CI/CD
├── Artifacts    → Packages
└── Test Plans   → Testing
```

## Git

```text
Working Directory
       ↓
git add
       ↓
Staging
       ↓
git commit
       ↓
Local Repository
       ↓
git push
       ↓
Remote Repository
```

## Get changes

```text
Remote
  ↓
git pull
  ↓
Local
```

## Branches

```text
main
 │
 ├── feature/A
 ├── feature/B
 └── bugfix/C
```

## Merge

```text
feature
   ↓
merge
   ↓
main
```

## Rebase

```text
main
   ↓
rebase
   ↓
feature commits replayed on latest main
```

## Most important commands

```bash
git init
git clone <url>

git status

git add .
git commit -m "message"

git push
git pull
git fetch

git branch
git switch main
git switch -c feature/name

git merge feature/name
git rebase main
```

### One-line memory

> **Azure DevOps manages the development lifecycle; Git manages source-code history; Azure Repos hosts Git repositories; Boards manages work; Pipelines automates CI/CD; Artifacts manages packages; and Test Plans manages testing.**
