import css from './states.module.css'

const NoQuery = () => {
  return (
    <div className={css.wrapper}>
      <p>
        Welcome traveller,
        <br />
        please enter a query to begin the search.
      </p>
    </div>
  )
}

export default NoQuery
