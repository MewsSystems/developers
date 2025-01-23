import { ReactNode } from "react"

interface LayoutProps {
  children: ReactNode
}

export const Layout = ({ children }: LayoutProps) => {
  return (
    <main className="flex h-full justify-center">
      <div className="container bg-white p-4 sm:p-6">{children}</div>
    </main>
  )
}

export default Layout
