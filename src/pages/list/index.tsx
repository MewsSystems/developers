import { Button, Image, Input, Rate, Table } from "antd"
import { SearchOutlined } from "@ant-design/icons"
import { useNavigate, useSearchParams } from "react-router-dom"
import { useForm, Controller } from "react-hook-form"
import { useGetMoviesQuery } from "../../api/movie"
import { useState } from "react"
import type { ColumnsType } from "antd/es/table"
import { MovieListItem } from "../../types"
import styled from "styled-components"
import {
  FALLBACK_IMAGE,
  IMAGE_URL_PREFIX,
  MOVIE_FETCH_FAILED_MESSAGE,
} from "../../const"
import { useFailedRequest } from "../../hooks"

interface SearchForm {
  searchValue: string
}

export const MovieList = () => {
  const navigate = useNavigate()
  let [searchParams, setSearchParams] = useSearchParams({
    searchValue: "",
    page: "1",
  })
  const [page, setPage] = useState(1)
  const searchValue = searchParams.get("searchValue") || ""
  const { control, setValue } = useForm<SearchForm>({
    defaultValues: {
      searchValue,
    },
  })

  const {
    data: movies,
    isError,
    isFetching,
  } = useGetMoviesQuery(
    { searchValue, page },
    { skip: searchValue.trim().length === 0 },
  )
  useFailedRequest(isError, MOVIE_FETCH_FAILED_MESSAGE)

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchParams({ searchValue: e.target.value, page: "1" })
    setValue("searchValue", e.target.value)
  }

  const handlePageChange = (page: number) => {
    setPage(page)
    setSearchParams({ searchValue, page: page.toString() })
    window.scrollTo({
      top: 0,
      behavior: "smooth",
    })
  }

  const columns: ColumnsType<MovieListItem> = [
    {
      title: "Poster",
      dataIndex: "poster_path",
      key: "posterPath",
      render: (posterPath) => (
        <Image
          width={88}
          height={88}
          style={{ objectFit: "cover" }}
          src={`${IMAGE_URL_PREFIX}${posterPath}`}
          fallback={FALLBACK_IMAGE}
          alt="poster"
        />
      ),
    },
    {
      title: "Name",
      dataIndex: "title",
      key: "title",
      render: (_, { id, title }) => (
        <Button type="link" onClick={() => navigate(`/movie/${id}`)}>
          {title}
        </Button>
      ),
    },
    {
      title: "Description",
      dataIndex: "overview",
      key: "overview",
      render: (overview) => <DescriptionCell>{overview}</DescriptionCell>,
    },
    {
      title: "Popularity",
      dataIndex: "popularity",
      key: "popularity",
      align: "center",
      responsive: ["lg"],
      render: (popularity) => (
        <StyledRate disabled defaultValue={popularity / 10} />
      ),
    },
    {
      title: "Release date",
      dataIndex: "release_date",
      key: "release_date",
      responsive: ["lg"],
      render: (releaseDate) => new Date(releaseDate).toLocaleDateString(),
    },
  ]

  return (
    <>
      <Title>Mews Movie Search</Title>
      <Controller
        name="searchValue"
        control={control}
        render={({ field }) => (
          <Input
            {...field}
            onChange={handleSearchChange}
            size="large"
            placeholder="Search by movie name"
            prefix={<SearchOutlined />}
          />
        )}
      />
      {movies && (
        <StyledTable
          columns={columns}
          dataSource={movies.results}
          loading={isFetching}
          pagination={{
            total: movies.total_results,
            onChange: handlePageChange,
            position: ["bottomCenter"],
            pageSize: 20,
            showSizeChanger: false,
          }}
        />
      )}
    </>
  )
}

const Title = styled.h1`
  margin-bottom: 0.5em;
  font-size: 3em;
  text-align: center;
  color: #000;
`

const StyledTable = styled(Table)`
  margin-top: 2em;
`

const StyledRate = styled(Rate)`
  display: flex;
  padding: 0 1em;
`

const DescriptionCell = styled.div`
  display: -webkit-box;
  -webkit-line-clamp: 4;
  -webkit-box-orient: vertical;
  overflow: hidden;
`
