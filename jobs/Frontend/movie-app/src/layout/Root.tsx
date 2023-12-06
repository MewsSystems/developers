import { Outlet, Link } from "react-router-dom"
import Header from "@/components/Header"
import Site from "@/components/Site"
import { Title } from "@/components/typography"

function Root() {
  return (
    <>
      <Header>
        <Link to="/">
          <Title>Movies app</Title>
        </Link>
      </Header>
      <Site>
        <Outlet />
      </Site>
    </>
  )
}

export default Root
