import { AuthCallbackRouteComponent } from '@/pages/auth/AuthCallback';
import { createFileRoute, redirect } from '@tanstack/react-router'

export const Route = createFileRoute('/authcallback')({
  loaderDeps: ({ search }) => {
    const request_token: string = (search as any).request_token;
    return { request_token }
  },
  loader: async ({ deps: { request_token }, context: { auth } }) => {
    // Grab the request token from the URL search params
    const requestToken = request_token;
    const isSessionCreated = await auth?.createSession(requestToken);
    if (isSessionCreated) {
      throw redirect({ to: "/movies" });
    }
    throw redirect({ to: "/login" });
  },
  component: AuthCallbackRouteComponent,
})

