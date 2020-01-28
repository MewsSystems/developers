import { NextPageContext } from 'next'
import Router from 'next/router'

export const serverRedirect = (ctx: NextPageContext, path: string) => {
    if (ctx.res) {
        ctx.res.writeHead(302, { Location: path })
        ctx.res.end()
    } else {
        Router.push(path)
    }
    return {}
}
