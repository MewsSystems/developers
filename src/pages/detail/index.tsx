import { useNavigate, useParams } from "react-router-dom"
import { useGetMovieDetailsQuery } from "../../api/movie"
import { Button } from "antd"
import { LeftOutlined } from "@ant-design/icons"

export const MovieDetail = () => {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()

  const { data: movie, isFetching } = useGetMovieDetailsQuery(Number(id))

  if (isFetching) {
    return <div>Loading...</div>
  }

  if (!movie) {
    return <div>Movie not found</div>
  }

  const { status, title } = movie

  return (
    <div>
      <Button icon={<LeftOutlined />} type="link" onClick={() => navigate(-1)}>
        Back
      </Button>
      <div>{title}</div>
      <div>
        <div>Status:</div>
        {status}
      </div>
    </div>
  )
}
