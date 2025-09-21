import { useQuery } from "@tanstack/react-query";
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { getAccountApi } from "../api/accountApi";

export default function useQueryAccount() {
  const auth = useAuth();
  return useQuery({
    queryKey: ["account", auth.sessionId],
    queryFn: () => getAccountApi({ session_id: auth.sessionId + "" }),
  });
}
