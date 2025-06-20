import Logo from './assets/Logo.png';
import Codam from './assets/Codam.svg';
import Mews from './assets/Mews.png';

function Navbar()
{
	return (
		<nav className="h-[8vh] p-5 px-20 flex justify-between items-center">
			<img src={Logo} className="h-18 w-auto"/>
			<img src={Codam} className="hidden md:flex h-18 w-auto"/>
			<img src={Mews} className='hidden md:flex h-18 w-auto'/>
		</nav>
	)
}

export default Navbar