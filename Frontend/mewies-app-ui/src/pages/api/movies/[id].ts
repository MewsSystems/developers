import { NextApiRequest, NextApiResponse } from 'next'

export default (req: NextApiRequest, res: NextApiResponse) => {
    const {
        query: { id },
    } = req

    res.end(`Movie: ${id}`)
}
