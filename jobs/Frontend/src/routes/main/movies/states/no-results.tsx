import css from './states.module.css'

const NoResults = () => {
  return (
    <div className={css.wrapper}>
      <p>
        Your query matches no results.
        <br />
        Please try changing your query.
      </p>
    </div>
  )
}

export default NoResults
