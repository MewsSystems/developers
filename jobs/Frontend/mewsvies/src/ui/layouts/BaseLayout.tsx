import React from 'react'
import styles from './BaseLayout.module.scss'

interface BaseLayoutProps {
    children: React.ReactNode
}

export default function MovieList({ children }: BaseLayoutProps) {
    return (
        <div className={styles.main}>
            <div className={styles.header}>
                <h1>MEWSVIES</h1>
            </div>
            <div className={styles.content}>{children}</div>
        </div>
    )
}
