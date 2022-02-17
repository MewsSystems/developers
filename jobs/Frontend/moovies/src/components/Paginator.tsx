import { useEffect, useState } from "react"

const Paginator = (props: any) => {

    const { maxPages, currPage, setCurrPage } = props

    const increasePage = () => {
        if (currPage + 1 > maxPages) {
            return
        }
        setCurrPage(currPage + 1)
    }

    const decreasePage = () => {
        if (currPage - 1 < 1) {
            return
        }
        setCurrPage(currPage - 1)
    }

    return (
        <div>
            <button onClick={decreasePage}>prev page</button>
            <span>current: {currPage}</span>
            <button onClick={increasePage}>next page</button>
        </div>
    )
}

export default Paginator
