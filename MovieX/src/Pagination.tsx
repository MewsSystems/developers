interface PaginationProps
{
	page: number;
	setPage: React.Dispatch<React.SetStateAction<number>>;
}

function Pagination({page, setPage}: PaginationProps)
{
	return (
		<div className='flex w-full items-center justify-center'>
					<div className="join flex w-full justify-between">
						{page > 1 ? <button className="hover:text-[#ffbd5a] rounded-l-3xl hover:cursor-pointer" onClick={() => setPage(prev => prev - 1)}>«</button> : <div></div>}
						<button>{page}</button>
						<button className="hover:text-[#ffbd5a] rounded-r-3xl hover:cursor-pointer" onClick={() => setPage(prev => prev + 1)}>»</button>
					</div>
				</div>
	)
}

export default Pagination