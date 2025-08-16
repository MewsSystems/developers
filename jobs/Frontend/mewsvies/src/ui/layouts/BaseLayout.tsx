import { Outlet } from 'react-router-dom'
import styles from './BaseLayout.module.scss'

export default function MovieList() {
    return (
        <div className={styles.main}>
            <div className={styles.header}>
                <h1>MEWSVIES</h1>
            </div>
            <div className={styles.content}>
                <Outlet />
            </div>
        </div>
    )
}
