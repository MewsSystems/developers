import { TailSpin } from 'react-loader-spinner'
import styles from './Loading.module.scss'

export default function Loading() {
    return (
        <div className={styles['loading-overlay']}>
            <TailSpin color="#00BFFF" height={80} width={80} />
        </div>
    )
}
