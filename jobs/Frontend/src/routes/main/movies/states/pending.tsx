import Loading from 'components/loading/loading'
import css from './states.module.css'

const Pending = () => {
  return (
    <div className={css.wrapper}>
      <Loading />
    </div>
  )
}

export default Pending
