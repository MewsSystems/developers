import { Outlet } from "react-router-dom"
import Header from "@/components/Header"
import Site from "@/components/Site"
import { Title } from "@/components/typography"

function Root() {
  return (
    <>
      <Header>
        <Title>Movies app</Title>
      </Header>
      <Site>
        <Outlet />
      </Site>
    </>
  )
}

export default Root
