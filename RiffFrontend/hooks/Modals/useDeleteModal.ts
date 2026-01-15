import {create } from "zustand";

interface DeleteModalStore {
  isOpen: boolean;
  trackId: string;
  onOpen: (id: string) => void;
  onClose: () => void;
}

const useDeleteModal = create<DeleteModalStore>((set) => ({
  isOpen: false,
  trackId: "",
  onOpen: (id: string) => set({ isOpen: true, trackId: id }),
  onClose: () => set({ isOpen: false, trackId: "" }),
}));


export default useDeleteModal;