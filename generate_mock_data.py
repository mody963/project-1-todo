# import os
# import json
# import random
# from datetime import datetime, timedelta

# ROOT = os.path.dirname(os.path.abspath(__file__))
# TASKS_DIR = os.path.join(ROOT, "jsontasks")
# PERSONS_FILE = os.path.join(ROOT, "Persons.json")
# ALLOCATIONS_FILE = os.path.join(ROOT, "Allocations.json")

# PERSON_NAMES = [
#     "Fernando", "Aimee", "Mouhamad", "Iris", "Noah", "Lina", "Milan", "Sara", "Jules", "Eva"
# ]

# PRIORITIES = ["must have", "should have", "could have"]
# STATUSES = ["to do", "in progress", "completed"]

# SUBJECTS = [
#     "server setup", "database cleanup", "email campaign", "user onboarding", "report generation",
#     "UI redesign", "mobile testing", "data export", "security audit", "log review",
#     "social media plan", "customer follow-up", "product training", "inventory check", "deployment review",
#     "meeting summary", "performance tuning", "bug triage", "invoice processing", "content update",
#     "calendar sync", "expense audit", "system backup", "marketing brief", "customer survey"
# ]

# VERBS = [
#     "Complete", "Review", "Prepare", "Design", "Test", "Schedule", "Validate", "Document", "Optimize", "Confirm",
#     "Build", "Analyze", "Deploy", "Research", "Update", "Organize", "Approve", "Fix", "Plan", "Publish"
# ]

# OBJECTS = [
#     "landing page", "product roadmap", "notification flow", "report dashboard", "email template",
#     "user journey", "error handling", "analytics dashboard", "security settings", "integration test",
#     "team sync", "customer feedback", "resource plan", "market analysis", "training guide"
# ]


# def make_safe_name(text: str) -> str:
#     safe = ''.join(c if c.isalnum() else '_' for c in text).strip('_')
#     return safe or 'task'


# def dump_json(path, data):
#     with open(path, 'w', encoding='utf-8') as f:
#         json.dump(data, f, indent=2, ensure_ascii=False)


# def make_dependant(ids):
#     return {
#         "array": ids,
#         "Count": len(ids),
#         "Dirty": False
#     }


# def build_task(id_: int, description: str, priority: str, status: str, dependant_ids):
#     return {
#         "Id": id_,
#         "Description": description,
#         "Priority": priority,
#         "dependant": make_dependant(dependant_ids),
#         "Status": status,
#         "Completed": status == "completed",
#         "CreationDate": (datetime.now() - timedelta(days=random.randint(0, 90), hours=random.randint(0, 23), minutes=random.randint(0, 59))).isoformat()
#     }


# def build_person(id_, name):
#     return {"Id": id_, "Name": name}


# def build_allocation(task, person):
#     return {"Task": task, "Person": person}


# def main():
#     if os.path.isdir(TASKS_DIR):
#         for filename in os.listdir(TASKS_DIR):
#             if filename.endswith('.json'):
#                 os.remove(os.path.join(TASKS_DIR, filename))
#     else:
#         os.makedirs(TASKS_DIR, exist_ok=True)

#     persons = [build_person(i + 1, name) for i, name in enumerate(PERSON_NAMES)]
#     dump_json(PERSONS_FILE, persons)

#     tasks = []
#     for task_id in range(1, 1001):
#         verb = random.choice(VERBS)
#         subject = random.choice(SUBJECTS if random.random() < 0.7 else OBJECTS)
#         description = f"{verb} {subject} #{task_id}"
#         priority = random.choices(PRIORITIES, weights=[0.35, 0.40, 0.25], k=1)[0]
#         status = random.choices(STATUSES, weights=[0.45, 0.35, 0.20], k=1)[0]

#         # Chain dependencies in groups of 5 tasks, plus occasional cross-group references
#         base = ((task_id - 1) // 5) * 5 + 1
#         dependant_ids = []
#         if task_id > base:
#             max_deps = min(3, task_id - base)
#             dependant_ids = list(range(base, base + random.randint(1, max_deps)))

#         if task_id % 50 == 0 and task_id > 5:
#             cross_id = random.randint(1, task_id - 5)
#             if cross_id not in dependant_ids:
#                 dependant_ids.append(cross_id)

#         task = build_task(task_id, description, priority, status, dependant_ids)
#         tasks.append(task)

#         filename = f"{task_id}_{make_safe_name(description)}.json"
#         dump_json(os.path.join(TASKS_DIR, filename), task)

#     allocations = []
#     for task in tasks:
#         task_id = task["Id"]
#         person = persons[(task_id - 1) % len(persons)]
#         allocations.append(build_allocation(task, person))
#         if task_id % 10 == 0:
#             extra_person = persons[(task_id // 10) % len(persons)]
#             if extra_person["Id"] != person["Id"]:
#                 allocations.append(build_allocation(task, extra_person))

#     dump_json(ALLOCATIONS_FILE, allocations)
#     print("Generated 1000 tasks in jsontasks/ and updated Persons.json + Allocations.json")


# if __name__ == '__main__':
#     main()
