import Logo from '@/components/Logo';

const Footer = () => {
  return (
    <footer className="border-t h-24 border-slate-300/30 bg-primary px-6">
      <div className="container mx-auto h-full flex items-center justify-between">
        <Logo className="w-[120px] text-white" />
        <div className="text-white font-light">Fran Carmona</div>
      </div>
    </footer>
  );
};

export default Footer;
