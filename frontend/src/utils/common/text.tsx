export const formatFirstName = (fullName: string): string => {
  if (!fullName) return "";
  const firstName = fullName.trim().split(" ")[0];
  return firstName.charAt(0).toUpperCase() + firstName.slice(1).toLowerCase();
};

export const formatNames = (fullName: string): string => {
  if (!fullName) return "";
  return fullName.charAt(0).toUpperCase() + fullName.slice(1).toLowerCase();
};

export const normalize = (str?: string | null) =>
  (str ?? "")
    .normalize("NFD")
    .replace(/\p{Diacritic}/gu, "")
    .toLowerCase()
    .trim();
