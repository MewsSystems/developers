import React from "react"
import { requestTokenApi } from "@/entities/auth/api/requestTokenApi";
import { createSessionApi } from "@/entities/auth/api/createSessionApi";
import { getAccountApi } from "@/entities/account/api/accountApi";
import { useLocalStorage } from "@uidotdev/usehooks";

export interface AuthContext {
    isAuthenticated: boolean
    accountId: number | null;
    sessionId: string | null;
    login: () => Promise<void>
    logout: () => Promise<void>
    createSession: (requestToken: string) => Promise<boolean>;
}

const AuthContext = React.createContext<AuthContext | null>(null)

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [sessionId, setSessionId] = useLocalStorage<string | null>('sessionId');
    const [accountId, setAccountId] = useLocalStorage<number | null>('accountId');

    const isAuthenticated = !!sessionId;

    const login = React.useCallback(async () => {
        const data = await requestTokenApi();
        if (data.success) {
            // Redirect user to TMDB to approve the request token
            window.location.href = openPermissionPageTMDB(data.request_token, `${window.location.origin}/authcallback`);
        }
    }, []);

    const logout = React.useCallback(async () => {
        // Clear session data from localStorage
        setSessionId(null);
        setAccountId(null)
        window.location.reload();

    }, []);

    const createSession = React.useCallback(async (requestToken: string) => {
        const data = await createSessionApi(requestToken);

        if (data.success) {
            // Store the session ID in localStorage
            setSessionId(data.session_id)
            // Get the account ID and store it
            const accountData = await getAccountApi({ session_id: data.session_id });
            if (accountData.id) {
                setAccountId(parseInt(accountData.id))
            }
            return true;
        }
        return false;

    }, []);

    return (
        <AuthContext.Provider value={{ isAuthenticated, accountId, sessionId, createSession, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth() {
    const context = React.useContext(AuthContext)
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider')
    }
    return context
}

function openPermissionPageTMDB(requestToken: string, redirectCallback: string) {
    return `https://www.themoviedb.org/authenticate/${requestToken}?redirect_to=${redirectCallback}`
}