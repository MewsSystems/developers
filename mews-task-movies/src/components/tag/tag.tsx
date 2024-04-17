import './tag.css';

export default function Tag({ name }: { name: string }) {
  return <span className="tag">{name}</span>;
}
