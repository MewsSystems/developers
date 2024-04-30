import { Form, Link, useSearchParams, useSubmit } from "@remix-run/react"
import { setNewSearchParams } from "~/utils/set-new-search-params"
import styles from "./_pagination.module.css"
import { PaginationLink } from "~/components/pagination/_pagination-link"
import {
  PAGE_FIELD_NAME,
  PAGE_PARAM_NAME,
  SEARCH_FIELD_NAME,
  SEARCH_PARAM_NAME,
} from "~/constants"

const MIN_PAGE = 1

type Props = {
  totalPages: number
}

export const Pagination = ({ totalPages }: Props) => {
  const submit = useSubmit()
  const [searchParams] = useSearchParams()

  const searchQuery = searchParams.get(SEARCH_PARAM_NAME) ?? ""
  const currentPage = Number(searchParams.get(PAGE_PARAM_NAME) ?? "1")

  const previousPageSearch = setNewSearchParams(searchParams, {
    page: `${currentPage - 1}`,
  }).toString()

  const nextPageSearch = setNewSearchParams(searchParams, {
    page: `${currentPage + 1}`,
  }).toString()

  const handleBlur = (event: React.FormEvent<HTMLFormElement>) => {
    const form = event.currentTarget
    submit(form)
  }

  return (
    <section className={styles.container}>
      <PaginationLink
        disabled={currentPage === MIN_PAGE}
        to={{
          pathname: "/",
          search: previousPageSearch,
        }}
      >
        Previous
      </PaginationLink>

      <Form
        method={"get"}
        action={"/"}
        className={styles.form}
        onBlur={handleBlur}
      >
        <input
          type="hidden"
          name={SEARCH_FIELD_NAME}
          defaultValue={searchQuery}
        />
        <section className={styles.inputWrapper}>
          <span>Page</span>
          <input
            className={styles.input}
            disabled={MIN_PAGE === totalPages}
            key={currentPage}
            name={PAGE_FIELD_NAME}
            type="number"
            defaultValue={currentPage}
            min={MIN_PAGE}
            max={totalPages}
            data-testid={"page-input"}
          />
          <span>of {totalPages}</span>
        </section>
        <button className={styles.hiddenSubmitButton} type="submit">
          Go
        </button>
      </Form>

      <PaginationLink
        disabled={currentPage === totalPages}
        to={{
          pathname: "/",
          search: nextPageSearch,
        }}
      >
        Next
      </PaginationLink>
    </section>
  )
}
