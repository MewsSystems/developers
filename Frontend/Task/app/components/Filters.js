import React from 'react'
import styles from './css/Filters.scss'

const Filters = ({trendCheckers, onChange, activeFilters}) => {
	return (
		<div className={styles.container}>
			{
				Object.keys(trendCheckers).map((filter,i) => {
					const isActive = activeFilters.indexOf(filter) < 0 
					return (
						<div key={i}>
							<input type='checkbox' value={filter} onChange={e => onChange(e)} defaultChecked={isActive}/>
							<label>Show {filter} trends</label>
						</div>
					)
				})
			}
		</div>
	)
}

export default Filters