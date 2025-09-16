import { Login } from "@/pages/login";
import { createFileRoute } from "@tanstack/react-router";

export const Route = createFileRoute('/login' as any)({
    component: Login
})