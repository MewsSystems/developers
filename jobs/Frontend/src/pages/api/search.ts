// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from "next";
// import { request } from "graphql-request";

type Data = {
  name: string;
};

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse<Data>,
) {
  const response = await fetch(
    `https://api.themoviedb.org/3/search/movie?query=${req.query.query}&page=${req.query.page}&api_key=03b8572954325680265531140190fd2a`
  );
  const json = await response.json();
  res.status(200).json(json);
}
