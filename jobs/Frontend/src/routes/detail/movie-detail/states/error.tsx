import css from './states.module.css'

const Error = () => {
  return (
    <div className={css.wrapper}>
      <p>
        Uh oh, something went wrong.
        <br />
        Please try again later.
      </p>
    </div>
  )
}

export default Error
