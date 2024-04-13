import { HomeSearchContentProps, UseHomeSearchHook } from '../types'

export const useHomeSearchContent = (
    props: HomeSearchContentProps,
): UseHomeSearchHook => {
    return {
        ...props,
    }
}
