Dit project is een console‑gebaseerde To‑Do applicatie gebouwd in C#.
Het doel van de opdracht is om eigen datastructuren te implementeren en deze toe te passen in een werkende applicatie volgens Clean Architecture.

De applicatie ondersteunt:

Taken toevoegen

Taken verwijderen

Taken togglen (voltooid / niet voltooid)

Opslag in JSON

Een console‑interface met Spectre.Console

Een volledig zelfgeschreven ArrayCollection + Iterator

Een volledig zelfgeschreven LinkedList + Iterator

Een volledig zelfgeschreven Hashmaps + Wrappers

Een volledig zelfgeschreven bst




# ARCHITECTUUR

Het project volgt een eenvoudige variant van Clean Architecture, waarbij elke laag een duidelijke verantwoordelijkheid heeft.

1. Model Layer
Bevat alleen data‑objecten.

enum taskstatus en priority

Person:

    int Id
    string Name
Task_Allocation:
   TaskItem Task
    Person Person
TaskItem: 
    int Id
    string Description
    
    TaskPriority Priority

    TaskStatus _status
    
    IMyCollection<int> dependant

    TaskStatus Status

    bool Completed

    DateTime CreationDate


2. Collections Layer
Bevat de zelfgemaakte datastructuren die verplicht zijn voor de opdracht.

MyIterator<T> — interface voor iterators

MyArrayList<T> — dynamische array (vergelijkbaar met List<T>)

MyLinkedList<T> 

MyLinkedListiterator

MyHashMap

HashMapWrapper

MybinaryTree

MybinaryTreeiterator

3. interface
bevat alle interfaces

IMyCollection

IMyiteratable

IMyIterator


4. Repository Layer
Verantwoordelijk voor opslag en laden van data.

ITaskRepository — abstractie

JsongenericRepository — implementatie met JSON‑bestand gebruikt wrappers

GenericFactory - wrapper voor collections

IAllocationRepository- IPersonRepository - ITaskRepository 

Wrappers - implements the interfaces above 


5. Service Layer
Bevat alle business logic.

ITaskService

TaskService

Taken toevoegen

Taken verwijderen

Taken togglen

Automatisch ID genereren

Werkt uitsluitend via IMyCollection

IPersonService

PersonService

IAllocationService

AllocationService


6. View Layer
De gebruikersinterface.

ITaskView

ConsoleTaskView

Gebouwd met Spectre.Console

Mooie tabellen, prompts en kleuren

Roept alleen de Service‑laag aan

FilterTask - menu voor het filteren


7. Program.cs
De “composition root”:

Maakt repository → service → view

Start de applicatie

8. tests

bevat unittests per collection en test vooral imycollection

# Data Structuren

De kern van dit project is de ArrayCollection, een zelfgeschreven dynamische array.

Functionaliteit:
Automatisch vergroten van capaciteit

Elementen toevoegen

Elementen verwijderen

Zoeken met FindBy

Itereren met een eigen iterator

Converteren naar array voor JSON

Waarom geen List<T>?
De opdracht vereist dat alle datastructuren zelf worden geïmplementeerd.
Daarom wordt nergens gebruikgemaakt van:

List<T>

Dictionary<T>

LinkedList<T>

LINQ


# Console UI (Spectere.Console)

De applicatie gebruikt Spectre.Console voor een moderne en overzichtelijke interface.

Voorbeelden van UI‑elementen:

ASCII‑titel (FigletText)

Tabellen (Table)

Menu’s (SelectionPrompt)

Kleurrijke prompts (AnsiConsole.Ask)

Dit maakt de applicatie veel gebruiksvriendelijker en professioneler.


# Data

Alles wordt opgeslagen in Project/Data?tasks.json

# Het programma runnen

Run het programma door in de map waar Project.csproj file zit "dotnet run" te doen
