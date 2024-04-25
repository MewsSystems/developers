import Logo from '@/components/Logo';
import { Link } from 'react-router-dom';

const Header = () => {
  return (
    <header className="h-16 bg-white/75 border-b border-slate-900/10 flex items-center justify-between sticky top-0 px-4 backdrop-blur-xl">
      <Link to="/">
        <Logo className="w-[120px]" />
      </Link>
      <h1 className="text-xl tracking-wide">Movie Search</h1>
    </header>
  );
};

export default Header;
