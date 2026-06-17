import css from './grid.module.css'

interface GridItem {
  id: string | number
  title: string
}

interface GridProps<T extends GridItem> {
  list: T[]
  renderItem?: (item: NoInfer<T>) => React.ReactNode
}

const Grid = <T extends GridItem>({ list, renderItem }: GridProps<T>) => {
  return (
    <div className={css.grid}>
      {list.map((item) => (
        <div key={item.id} className={css.item}>
          {renderItem ? renderItem(item) : item.title}
        </div>
      ))}
    </div>
  )
}

export default Grid
