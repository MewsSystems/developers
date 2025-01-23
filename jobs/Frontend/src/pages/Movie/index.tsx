import { Route, Routes } from "react-router-dom"
import MovieListPage from "./List"

export const DevicePages = () => {
  return (
    <Routes>
      <Route path="/" Component={MovieListPage} />
    </Routes>
  )
}

export default DevicePages
