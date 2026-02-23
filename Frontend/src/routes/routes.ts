import type { RouteRecordRaw } from "vue-router";

const staticRoutes: Array<RouteRecordRaw> = [
  {
    path: "/login",
    name: "login",
    component: () => import("../pages/auth/login.vue"),
    meta: {
      title: "Login",
      auth: false,
    },
  },
  {
    path: "/register",
    name: "register",
    component: () => import("../pages/auth/register.vue"),
    meta: {
      title: "Register",
      auth: false,
    },
  },
];

const adminRoutes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "/",
    component: () => import("../layouts/AdminLayout.vue"),
    redirect: "/dashboard",
    meta: {
      isKeepAlive: true,
    },
    children: [
      {
        path: "/dashboard",
        name: "dashboard",
        component: () => import("../pages/dashboard/index.vue"),
        meta: {
          title: "Dashboard",
          isKeepAlive: true,
          roles: ["admin", "common"],
          icon: "mdi-view-dashboard-outline",
        },
      },
      {
        path: "/note",
        name: "notes",
        component: () => import("../pages/notes/index.vue"),
        meta: {
          title: "Notes",
          isKeepAlive: true,
          roles: ["admin", "common"],
          icon: "mdi-note-outline",
        },
      },
    ],
  },
];

const notFoundAndNoPower: Array<RouteRecordRaw> = [
  {
    path: "/:pathMatch(.*)*",
    name: "notFound",
    component: () => import("../pages/[...all].vue"),
    meta: {
      title: "404 Not Found",
      isHide: true,
    },
  },
];

export { adminRoutes, notFoundAndNoPower, staticRoutes };
