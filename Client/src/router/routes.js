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
        component: () => import("pages/DashboardPage.vue"),
      },
      {
        path: "/home/emptySprint",
        component: () => import("pages/EmptySprintPage.vue"),
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
