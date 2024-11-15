import { Fragment, ReactNode } from "react"
import { Dialog, DialogPanel, Transition, TransitionChild } from "@headlessui/react"
import { twMerge } from "tailwind-merge"

interface ModalProps {
  className?: string
  children?: ReactNode
  isOpen?: boolean
  onDismiss?: () => void
}

export const Modal = ({ className, children, isOpen, onDismiss }: ModalProps) => {
  return (
    <Transition as={Fragment} show={isOpen}>
      <Dialog
        as="div"
        className="relative z-30"
        onClose={() => {
          onDismiss?.()
        }}
      >
        <TransitionChild
          as={Fragment}
          enter="ease-out duration-300"
          enterFrom="opacity-0"
          enterTo="opacity-100"
          leave="ease-in duration-200"
          leaveFrom="opacity-100"
          leaveTo="opacity-0"
        >
          <div className="fixed inset-0 bg-black bg-opacity-75 transition-opacity" />
        </TransitionChild>

        <div className="fixed inset-0 z-30 overflow-y-auto">
          <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
            <TransitionChild
              as={Fragment}
              enter="ease-out duration-300"
              enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
              enterTo="opacity-100 translate-y-0 sm:scale-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100 translate-y-0 sm:scale-100"
              leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
            >
              <DialogPanel
                className={twMerge(
                  "relative w-96 transform overflow-hidden rounded-lg bg-white p-4 text-left shadow-xl transition-all sm:w-[540px] md:w-[720px] md:p-6",
                  className,
                )}
              >
                {children}
              </DialogPanel>
            </TransitionChild>
          </div>
        </div>
      </Dialog>
    </Transition>
  )
}

export default Modal
