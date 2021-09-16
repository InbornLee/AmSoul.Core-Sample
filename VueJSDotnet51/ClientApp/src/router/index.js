import { createRouter, createWebHistory } from "vue-router";


const routes = [{
    path: "/",
    name: "Home",
    component: () => import("@/views/Home.vue"),
},
{
    path: "/Team",
    name: "Team",
    component: () => import("@/views/Team.vue"),
},
{
    path: "/About",
    name: "About",
    component: () =>
        import("@/views/About.vue"),
},
{
    path: "/Counter",
    name: "Counter",
    component: () =>
        import("@/views/Counter.vue"),
},
{
    path: "/FetchData",
    name: "FetchData",
    component: () =>
        import("@/views/FetchData.vue"),
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export default router;