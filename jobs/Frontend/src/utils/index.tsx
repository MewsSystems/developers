/* eslint-disable @typescript-eslint/no-explicit-any */
import type { ComponentPropsWithoutRef, FC } from 'react'

/**
 * Creates an implementation of a component using a View and a Controller hook
 * @param View A view component
 * @param useController A controller hook, can be parametrized with an object parameter
 *
 * Any parameters passed to controller will be passed as props to the resulting component
 */

export const wrap =
    <
        V extends FC<any>,
        C extends (argsObj: any) => ComponentPropsWithoutRef<V>,
    >(
        View: V,
        useController: C,
    ): FC<Parameters<C>[0]> =>
    ({ children, ...controllerArgs }) => {
        const AnyView = View as any
        const controller = useController(controllerArgs)
        return <AnyView {...controller}>{children}</AnyView>
    }
