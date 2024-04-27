import { Link } from 'react-router-dom';
import Logo from '@/components/Logo';

const Header = () => {
  return (
    <header className="h-16 bg-white/75 border-b border-slate-900/10 flex items-center sticky top-0 px-4 backdrop-blur-xl">
      <Link to="/">
        <Logo className="w-[120px]" />
      </Link>
    </header>
  );
};

export default Header;
