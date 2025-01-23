import { ReactNode } from "react"
import { Modal } from "."
import { ModalBody } from "./ModalBody"
import { ModalFooter } from "./ModalFooter"
import { ModalHeader } from "./ModalHeader"

interface ConfirmationModalProps {
  title: string
  content: ReactNode
  isOpen?: boolean
  onDismiss?: () => void
  okButton: ReactNode
  cancelButton: ReactNode
}

export const ConfirmationModal = ({
  title,
  content,
  isOpen = false,
  onDismiss,
  okButton,
  cancelButton,
}: ConfirmationModalProps) => {
  return (
    <Modal isOpen={isOpen} onDismiss={onDismiss}>
      <ModalHeader>{title}</ModalHeader>
      <ModalBody>{content}</ModalBody>
      <ModalFooter>
        <div className="mt-4 flex flex-col space-y-2 sm:flex-row sm:justify-end sm:space-x-2 sm:space-y-0">
          {cancelButton}
          {okButton}
        </div>
      </ModalFooter>
    </Modal>
  )
}

export default ConfirmationModal
