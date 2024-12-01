import { styled } from 'styled-components'

type CardProps = {
  children?: React.ReactNode
  isSkeleton?: boolean
}

function Card({ children, isSkeleton }: CardProps) {
  if (isSkeleton) {
    return (
      <CardSkeleton>
        <div className="details">
          <CardSkeletonChild style={{ height: '40%' }} />
          <CardSkeletonChild style={{ height: '1%' }} />
          <CardSkeletonChild style={{ height: '1%', width: '70%' }} />
        </div>
        <div className="actions">
          <CardSkeletonChild style={{ height: '1%', width: '10%' }} />
        </div>
      </CardSkeleton>
    )
  }

  return <StyledCard>{children}</StyledCard>
}

const StyledCard = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
  background-color: var(--primary-brand-color-100);
  padding: 0.2rem;
  border-radius: 0.8rem;
  height: 400px;
  overflow: hidden;
`

const CardSkeleton = styled(StyledCard)`
  justify-content: space-between;
  padding: 0.8rem;

  .details {
    flex-grow: 1;
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }

  .actions {
    display: flex;
    flex-direction: row-reverse;
  }
`

const CardSkeletonChild = styled(CardSkeleton)`
  background-color: var(--primary-brand-color-200);
  animation: pulse 2s cubic-bezier(0.4, 0, 0.6, 1) infinite;

  @keyframes pulse {
    0%,
    100% {
      opacity: 1;
    }
    50% {
      opacity: 0.5;
    }
  }
`

export default Card
