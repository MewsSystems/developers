import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { stringify } from "querystring";
import { useCallback } from "react";

export const useSearchParamsReplace = () => {
    const router = useRouter();
    const pathname = usePathname();
    const searchParams = useSearchParams();

    const replace = useCallback(
        (params: Record<string, string>) => {
            const urlSearchParams = new URLSearchParams(
                Array.from(searchParams.entries()),
            );

            for (const [name, value] of Object.entries(params)) {
                if (value) {
                    urlSearchParams.set(name, value);
                } else {
                    urlSearchParams.delete(name);
                }
            }

            let query = urlSearchParams.toString();

            if (query) {
                query = `?${query}`;
            }

            router.replace(`${pathname}${query}`);
        },
        [searchParams, pathname, router],
    );

    return { replace };
};
