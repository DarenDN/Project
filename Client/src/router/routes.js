const routes = [
  {
    path: "/home",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      {
        path: "",
        component: () => import("pages/IndexPage.vue"),
      },
      {
        path: "/home/sprint-info",
        component: () => import("pages/SprintInfo.vue"),
      },
      {
        path: "/home/sprint-backlog",
        component: () => import("pages/SprintBacklogPage.vue"),
      },
      {
        path: "/home/dashboard",
        component: () => import("pages/DashboardPage.vue")
      },
      {
        path: "/home/dashboard/task",
        component: () => import("pages/TaskPage.vue"),
      },
      {
        path: "/home/empty-sprint",
        component: () => import("pages/EmptySprintPage.vue"),
      },
      {
        path: "/home/project-info",
        component: () => import("pages/ProjectInfoPage.vue"),
      },
      {
        path: "/home/project-backlog",
        component: () => import("pages/ProjectBacklogPage.vue"),
      },
      {
        path: "/home/users",
        component: () => import("pages/UsersPage.vue"),
      },
      {
        path: "/home/meeting",
        component: () => import("pages/MeetingPage.vue"),
      }
    ],
  },
  {
    path: "/login",
    component: () => import("layouts/UndefinedLayout.vue"),
    children: [
      {
        path: "",
        component: () => import("pages/AuthenticationPage.vue"),
      },
    ]
  },
  {
    path: "/:catchAll(.*)*",
    component: () => import("layouts/UndefinedLayout.vue"),
    children: [
      {
        path: "",
        component: () => import("pages/ErrorNotFound.vue"),
      },
    ],
  },
];

export default routes;
