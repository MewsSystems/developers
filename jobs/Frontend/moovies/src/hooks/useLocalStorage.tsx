import { useState, useEffect } from "react"

const useLocalStorage = (operationType: string, storageKey: string, storeData: any) => {

    const [data, setData] = useState()

    const saveToStorage = async () => {
        try {
            await localStorage.set()
        }
        catch (err) {
            console.error(err)
        }
    }

    useEffect(() => {

        switch (operationType) {
            case "set":
                try {
                    localStorage.set()
                }
                catch (err) {
                    console.warn(err)
                }
        }

    })



}

export default useLocalStorage