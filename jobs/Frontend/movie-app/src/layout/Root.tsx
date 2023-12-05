import { Outlet } from "react-router-dom"

function Root() {
  return (
    <div>
      <nav>Movies app</nav>
      <Outlet />
    </div>
  )
}

export default Root
