import { useParams } from 'react-router-dom'

export const MovieDetailPage = () => {
    const { id } = useParams()
    return <div>MovieDetailPage - {id}</div>
}
