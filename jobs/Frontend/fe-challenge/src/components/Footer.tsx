import Logo from '@/components/Logo';

const Footer = () => {
  return (
    <footer className="border-t h-24 border-slate-300/30 bg-gray-900 px-6">
      <div className="container mx-auto h-full flex items-center">
        <Logo className="w-[120px] text-white" />
      </div>
    </footer>
  );
};

export default Footer;
